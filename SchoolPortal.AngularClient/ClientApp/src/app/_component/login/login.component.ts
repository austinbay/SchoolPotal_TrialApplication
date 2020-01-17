// login.component.ts
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';


@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

	model: any = {};
	loading = false;
 

	constructor(private router: Router, private authService: AuthService) {


	}

	ngOnInit() {
		this.authService.logout();
	}

	submitForm() {	
		this.loading = true;		 
		this.authService.login(this.model).subscribe((response: any) => {
			this.loading = false;
			if (response.isSuccess) {
				this.authService.storeTokens(response.data);
				this.router.navigate(['/studentrecord']);		
			} else {
				Swal.fire('Error!', response.errorMessage, 'error');
			}
			 
		});
	}

}
