

import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
	providedIn: 'root'
})
export class ErrorService {

	getClientErrorMessage(error: Error): string {
		return error.message ? error.message : error.toString();
	}

	getServerErrorMessage(error: HttpErrorResponse): string {

		if (!/localhost/.test(document.location.host)) {
			 if (!navigator.onLine) {
			    return 'No Internet Connection';
		    }
		}
		return error.error ? error.error : 'Internal Server Error';
	}
}
