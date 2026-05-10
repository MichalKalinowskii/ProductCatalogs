import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CatalogResponse } from '../../models/catalogs/catalogResponse';
import { ProductService } from '../../services/products/productService';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductResponse } from '../../models/products/productResponse';
import { ProductRequest } from '../../models/products/productRequest';
import { ProductDetailResponse } from '../../models/products/productDetailResponse';

type CreateProductForm = FormGroup<{
  catalogId: FormControl<string>;
  name: FormControl<string>;
  price: FormControl<number>;
  description: FormControl<string | null>;
  code: FormControl<string | null>;
}>;

@Component({
  selector: 'app-product-form',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './product-form.html',
  styleUrl: './product-form.css',
})
export class ProductForm {

  @Input() catalogs: CatalogResponse[] = [];
  @Output() productCreated = new EventEmitter<any>();
  @Output() close = new EventEmitter<void>();
  
  public readonly createForm: CreateProductForm;
  
  constructor(private formBuilder: FormBuilder, private productService: ProductService) 
  {
    this.createForm = this.formBuilder.group({
      catalogId: this.formBuilder.nonNullable.control('', [Validators.required]),
      name: this.formBuilder.nonNullable.control('', [Validators.required]),
      price: this.formBuilder.nonNullable.control(0, [Validators.required, Validators.min(0)]),
      description: this.formBuilder.control(''),
      code: this.formBuilder.control(''),
    });
  }
  
  public isInvalid(control: FormControl<unknown>): boolean {
    return control.invalid && (control.dirty || control.touched);
  }

  public onSubmit(): void {
    if (this.createForm.invalid) {
      this.createForm.markAllAsTouched();
      return;
    }

    this.createProduct();
  }

  private createProduct(): void {
    const newProduct: ProductRequest = {
      catalogId: this.createForm.controls.catalogId.value,
      name: this.createForm.controls.name.value,
      price: this.createForm.controls.price.value,
      description: this.createForm.controls.description.value || '',
      code: this.createForm.controls.code.value || '',
    };

    this.productService.createProduct(newProduct).subscribe({
      next: productId => {
        this.createForm.reset();
        this.productCreated.emit({productId: productId, name: newProduct.name, price: newProduct.price, catalogId: newProduct.catalogId});
        this.close.emit();
      },
      error: (error) => {
        console.log('Błąd podczas tworzenia produktu:', error);
      }
    });
  }
}
