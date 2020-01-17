// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { TokenData, WeatherForecast} from '../_models/user';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class AuthService {

	
	apiUrl: string;

	constructor(private router: Router, private http: HttpClient) {

		this.apiUrl = environment.basePath + 'api/Auth/';

	}

	// Http Options
	httpOptions = {
		headers: new HttpHeaders({
			'Content-Type': 'application/json'
		})
	};

	
	// Verify user credentials on server to get token
	login(data) {
		return this.http.post(this.apiUrl + 'Login', data, this.httpOptions);
		
	}
	
	refreshToken(data: any) {
		return this.http.post(this.apiUrl + 'RefreshToken', data, this.httpOptions);
		
	}
	register(data) {
		return this.http.post(this.apiUrl + 'Register', data, this.httpOptions);
	}
	confirmEmailAndSetPassword(data) {
		return this.http.post(this.apiUrl + 'ConfirmEmailAndSetPassword', data, this.httpOptions);
			
	}
	// After login save token and other values(if any) in localStorage
	storeTokens(resp: TokenData) {
		localStorage.setItem('userName', resp.firstName);
		localStorage.setItem('access_token', resp.accessToken);
		localStorage.setItem('refresh_token', resp.refreshToken);
	
	}

	// Checking if token is set
	isLoggedIn() {
		return this.getToken() != null;
	}

	// After clearing localStorage redirect to login screen
	logout() {
		localStorage.clear();
		this.router.navigate(['/auth/login']);
	}

	getToken() {

	return localStorage.getItem("access_token");
  }
	getRefreshToken() {

		return localStorage.getItem("refresh_token");
	}
	getUserName() {

		return localStorage.getItem("userName");
	}
	// Get data from server for Dashboard
	getData(): Observable<WeatherForecast> {
		return this.http.get<WeatherForecast>(this.apiUrl + 'GetWeatherForecast', this.httpOptions);
	
	}
}
