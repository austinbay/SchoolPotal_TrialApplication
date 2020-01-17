// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class StudentRecordService {

	apiUrl: string;

  // Http Options
	httpOptions = {
		headers: new HttpHeaders({
			'Content-Type': 'application/json'
		})
	};
	constructor(private http: HttpClient) {
		this.apiUrl = environment.basePath + 'api/StudentRecord/';
	
	}

  // Get data from server
	getAll() {
		return this.http.get(this.apiUrl + 'GetAll', this.httpOptions);
	}
	
	getItem(itemId: number) {
		return this.http.get(this.apiUrl + 'GetItem/' + itemId);
	
	}
	addItem(item) {
		return this.http.post(this.apiUrl + 'AddItem', JSON.stringify(item), this.httpOptions);
		
	}
	updateItem(item: any) {
		return this.http.put(this.apiUrl + 'UpdateItem', JSON.stringify(item), this.httpOptions);
			
	}
	deleteItem(itemId: number) {
		return this.http.delete(this.apiUrl + 'DeleteItem/' + itemId);
	}
	
	
}
