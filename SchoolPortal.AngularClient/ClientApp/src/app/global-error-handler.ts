

import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { LoggingService } from './_helpers/logging.service';
import { ErrorService } from './_helpers/error.service';
import { AlertService } from './_helpers/alert.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {

	constructor(private injector: Injector) { }

	handleError(error: Error | HttpErrorResponse) {
		const errorService = this.injector.get(ErrorService);
		const logger = this.injector.get(LoggingService);
		const alertService = this.injector.get(AlertService);

		let message;
		let stackTrace;
		if (error instanceof HttpErrorResponse) {
			// Server error
			message = errorService.getServerErrorMessage(error);
			//stackTrace = errorService.getServerErrorStackTrace(error);
			alertService.showErrorMessage(message);
		} else {
			// Client Error
			message = errorService.getClientErrorMessage(error);
			alertService.showErrorMessage(message);
		}
		// Always log errors
		//logger.logError(message, stackTrace);
		 console.error(error);

		// IMPORTANT: Rethrow the error otherwise it gets swallowed
		throw error;
	}
}
