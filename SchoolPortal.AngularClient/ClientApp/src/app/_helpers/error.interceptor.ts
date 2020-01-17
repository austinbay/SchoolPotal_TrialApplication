
import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

import { AuthService } from '../_services/auth.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
	constructor(private authService: AuthService) { }

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		return next.handle(request).pipe(
			retry(1),
			catchError((error: any) => {
				if (error instanceof HttpErrorResponse) {
					// Server error
					if (error.status === 401) {
						// refresh token
					} else {
						return throwError(error);
					}
				} else {
					// Client Error
					return throwError(error);
				}
				
			})
		);    
		
	}
}
