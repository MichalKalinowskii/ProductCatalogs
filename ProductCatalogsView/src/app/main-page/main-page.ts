import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/products/productService';
import { CatalogService } from '../../services/catalogs/catalogService';
import { CatalogResponse } from '../../models/catalogs/catalogResponse';
import { CatalogRequest } from '../../models/catalogs/catalogRequest';
import { FormsModule } from '@angular/forms';
import { ProductForm } from "../product-form/product-form";
import { ProductResponse } from '../../models/products/productResponse';
import { ProductDetails } from "../product-details/product-details";
import { ProductDetailResponse } from '../../models/products/productDetailResponse';


@Component({
  selector: 'app-main-page',
  imports: [FormsModule, ProductForm, ProductDetails],
  templateUrl: './main-page.html',
  styleUrl: './main-page.css',
})
export class MainPage implements OnInit {

  public catalogs: CatalogResponse[] = [];
  public catalogProducts: { [catalogId: string]: ProductResponse[] } = {};
  public openedCatalogs: string[] = [];
  public editedCatalogId: string | null = null;
  public editedCatalogName: string = '';
  public isProductModalOpen: boolean = false;
  public selectedProductId: string | null = null;
  public isProductDetailsOpen: boolean = false;

  constructor(private productService: ProductService, private catalogService: CatalogService) { }

  public ngOnInit(): void {
    this.loadCatalogs();
  }

  public loadCatalogs() {
    this.catalogService.getCatalogs().subscribe({
      next: result => {
        this.catalogs = result;
      },
      error: error => {
        console.log('Wystąpił błąd podczas pobierania katalogów:', error);
      }
    });
  }

  public createCatalog(catalogName: string) {
    if (!catalogName.trim()) {
      return;
    }

    const newCatalog: CatalogRequest = { name: catalogName };
    this.catalogService.createCatalog(newCatalog)
      .subscribe({
        next: result => {
          this.loadCatalogs();
        },
        error: error => {
          console.log('Wystąpił błąd podczas tworzenia katalogu:', error);
        }
      });
  }

  public toggleCatalog(catalogId: string): void {
    const isOpened = this.openedCatalogs.includes(catalogId);

    if (isOpened) {
      this.openedCatalogs = this.openedCatalogs.filter(x => x !== catalogId);
      return;
    }

    this.openedCatalogs.push(catalogId);
    this.productService.getProductsInCatalog(catalogId).subscribe({
      next: result => {
        this.catalogProducts[catalogId] = result;
      },
      error: error => {
        console.log('Wystąpił błąd podczas pobierania produktów:', error);
      }
    });
  }

  public isCatalogOpened(catalogId: string): boolean {
    return this.openedCatalogs.includes(catalogId);
  }

  public startEditCatalog(catalog: CatalogResponse): void {
    this.editedCatalogId = catalog.catalogId;
    this.editedCatalogName = catalog.name;
  }

  public cancelEditCatalog(): void {
    this.editedCatalogId = null;
    this.editedCatalogName = '';
  }

  public updateCatalog(catalogId: string): void {
    if (!this.editedCatalogName.trim()) {
      return;
    }

    const updatedCatalog: CatalogRequest = {
      name: this.editedCatalogName
    };

    this.catalogService.updateCatalog(catalogId, updatedCatalog)
      .subscribe({
        next: () => {
          this.loadCatalogs();

          this.editedCatalogId = null;
          this.editedCatalogName = '';
        },
        error: error => {
          console.log('Wystąpił błąd podczas aktualizacji katalogu:', error);
        }
      });
  }

  public deleteCatalog(catalogId: string): void {
    this.catalogService.deleteCatalog(catalogId)
      .subscribe({
        next: () => {
          this.loadCatalogs();
        },
        error: error => {
          console.log('Wystąpił błąd podczas usuwania katalogu:', error);
        }
      });
  }

  public openProductModal(): void {
    this.isProductModalOpen = true;
  }

  public closeProductModal(): void {
    this.isProductModalOpen = false;
  }

  public onProductCreated(product: any): void {
    const catalogProducts = this.catalogProducts[product.catalogId];

    if (!catalogProducts) {
      return;
    }

    catalogProducts.push({
      productId: product.productId,
      name: product.name,
      price: product.price
    });
  }

  public deleteProduct(catalogId: string, productId: string): void {
    this.productService.deleteProduct(productId)
      .subscribe({
        next: () => {
          this.catalogProducts[catalogId] =
            this.catalogProducts[catalogId]
              .filter(product => product.productId !== productId);
        },
        error: error => {
          console.log('Błąd usuwania produktu:', error);
        }
      });
  }

  public canDeleteCatalog(catalogId: string): boolean {
    const products = this.catalogProducts[catalogId];
    if (!products) {
      return true;
    }

    return products.length === 0;
  }

  public openProductDetails(productId: string): void {
    this.selectedProductId = productId;
    this.isProductDetailsOpen = true;
  }

  public closeProductDetails(): void {
    this.selectedProductId = null;
    this.isProductDetailsOpen = false;
  }

  public onProductUpdated(updated: ProductDetailResponse): void 
  {
    for (const catalogId in this.catalogProducts) {
      this.catalogProducts[catalogId] =
        this.catalogProducts[catalogId]
          .filter(p => p.productId !== updated.productId);
    }
    if (!this.catalogProducts[updated.catalogId]) {
      this.catalogProducts[updated.catalogId] = [];
    }

    this.catalogProducts[updated.catalogId].push({
      productId: updated.productId,
      name: updated.name,
      price: updated.price
    });
  }
}
