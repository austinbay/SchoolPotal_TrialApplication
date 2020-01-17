import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2'



@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

	model: any = {};
	loading = false;
	

	constructor(private router: Router, private authService: AuthService) {


	}

	ngOnInit() {
		
  }
	submitForm() {

		this.loading = true;

		this.authService.register(this.model).subscribe((response: any) => {
			this.loading = false;
			if (response.isSuccess) { 
				this.model = {};
				Swal.fire(
					'Success!',
					'Your registration was successful! A confirmation link was sent to your email address.',
					'success'
				)
			} else {
				Swal.fire('Error!', response.errorMessage, 'error');
			}
			
		});
	}
}
