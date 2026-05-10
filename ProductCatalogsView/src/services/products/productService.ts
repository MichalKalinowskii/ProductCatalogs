import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductResponse } from '../../models/products/productResponse';
import { ProductDetailResponse } from '../../models/products/productDetailResponse';
import { ProductRequest } from '../../models/products/productRequest';

@Injectable({providedIn: 'root'})
export class ProductService {

    private readonly baseUrl = 'https://localhost:7063/api/Product';
    
    constructor(private http: HttpClient) { }
    
    getProductsInCatalog(catalogId: string): Observable<ProductResponse[]> {
        return this.http.get<ProductResponse[]>(`${this.baseUrl}/products/${catalogId}`);
    }

    getProductDetails(productId: string): Observable<ProductDetailResponse> {
        return this.http.get<ProductDetailResponse>(`${this.baseUrl}/details/${productId}`);
    }

    createProduct(product: ProductRequest): Observable<string> {
        return this.http.post<string>(`${this.baseUrl}/create`, product);
    }

    updateProduct(productId: string, product: ProductRequest): Observable<string> {
        return this.http.put<string>(`${this.baseUrl}/update/${productId}`, product);
    }

    deleteProduct(productId: string) {
        return this.http.delete(`${this.baseUrl}/delete/${productId}`);
    }
}