import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Project } from '../models/project';

@Injectable({ providedIn: 'root' })
export class ProjectService {
  private baseUrl = '/api/Projects';

  constructor(private http: HttpClient) {}

  list(page = 1, pageSize = 10): Observable<any> {
    return this.http.get(`${this.baseUrl}/List?page=${page}&pageSize=${pageSize}`);
  }

  get(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/Get?id=${id}`);
  }

  save(project: Project): Observable<any> {
    return this.http.post(`${this.baseUrl}/Save`, project);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Delete?id=${id}`);
  }
}
