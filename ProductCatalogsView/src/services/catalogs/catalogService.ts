import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CatalogResponse } from '../../models/catalogs/catalogResponse';
import { Observable } from 'rxjs';
import { CatalogRequest } from '../../models/catalogs/catalogRequest';

@Injectable({providedIn: 'root'})
export class CatalogService {

    private readonly baseUrl = 'https://localhost:7063/api/Catalog';

    constructor(private http: HttpClient) { }
    
    getCatalogs(): Observable<CatalogResponse[]> {
        return this.http.get<CatalogResponse[]>(`${this.baseUrl}/catalogs`);
    }

    createCatalog(catalog: CatalogRequest): Observable<string> {
        return this.http.post<string>(`${this.baseUrl}/create`, catalog);
    }

    updateCatalog(catalogId: string, catalog: CatalogRequest): Observable<string> {
        return this.http.put<string>(`${this.baseUrl}/update/${catalogId}`, catalog);
    }

    deleteCatalog(catalogId: string) {
        return this.http.delete(`${this.baseUrl}/delete/${catalogId}`);
    }
}