import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ProductService } from '../../services/products/productService';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CatalogResponse } from '../../models/catalogs/catalogResponse';
import { ProductDetailResponse } from '../../models/products/productDetailResponse';
import { ProductRequest } from '../../models/products/productRequest';

type EditProductForm = FormGroup<{
  catalogId: FormControl<string>;
  name: FormControl<string>;
  price: FormControl<number>;
  description: FormControl<string>;
  code: FormControl<string>;
}>;

@Component({
  selector: 'app-product-details',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css',
})
export class ProductDetails implements OnInit{

  @Input({ required: true }) public productId!: string;
  @Input() public catalogs: CatalogResponse[] = [];

  @Output() public close = new EventEmitter<void>();
  @Output() public productUpdated = new EventEmitter<ProductDetailResponse>();

  public product: ProductDetailResponse | null = null;
  public readonly form: EditProductForm;

  constructor(private formBuilder: FormBuilder, private productService: ProductService) 
  {
    this.form = this.formBuilder.group({
      catalogId: this.formBuilder.nonNullable.control('', Validators.required),
      name: this.formBuilder.nonNullable.control('', Validators.required),
      price: this.formBuilder.nonNullable.control(0, [Validators.required, Validators.min(0)]),
      description: this.formBuilder.nonNullable.control(''),
      code: this.formBuilder.nonNullable.control(''),
    });
  }

  ngOnInit(): void {
    this.loadProduct();
  }

  private loadProduct(): void {
    this.productService.getProductDetails(this.productId)
      .subscribe({
        next: result => {
          this.product = result;
          this.form.patchValue({
            catalogId: result.catalogId,
            name: result.name,
            price: result.price,
            description: result.description,
            code: result.code
          });
        },
        error: error => {
          console.log('Błąd pobierania produktu:', error);
        }
      });
  }

  public save(): void {
    if (this.form.invalid || !this.product) {
      return;
    }

    const request: ProductRequest = {
      catalogId: this.form.controls.catalogId.value,
      name: this.form.controls.name.value,
      price: this.form.controls.price.value,
      description: this.form.controls.description.value,
      code: this.form.controls.code.value,
    };

    this.productService
      .updateProduct(this.product.productId, request)
      .subscribe({
        next: productId => {
          const updatedProduct: ProductDetailResponse = {
            ...this.product!,
            ...request
          };

          this.productUpdated.emit(updatedProduct);
          this.close.emit();
        },
        error: error => {
          console.log('Błąd aktualizacji produktu:', error);
        }
      });
  }
}
