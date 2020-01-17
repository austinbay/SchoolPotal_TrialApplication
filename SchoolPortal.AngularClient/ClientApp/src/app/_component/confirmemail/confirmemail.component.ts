import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../_services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import Swal from 'sweetalert2'

@Component({
  selector: 'app-confirmemail',
  templateUrl: './confirmemail.component.html',
  styleUrls: ['./confirmemail.component.css']
})
export class ConfirmemailComponent implements OnInit {

	model: any = {};
	loading = false;
	 

	constructor(private router: Router, private avRoute: ActivatedRoute,
		   private authService: AuthService) {
	}

	ngOnInit() {
		if (this.avRoute.snapshot.params['userId']) {
			this.model.userId = this.avRoute.snapshot.params['userId'];
		}
		if (this.avRoute.snapshot.params['code']) {
			this.model.code = this.avRoute.snapshot.params['code'];
		}
	}
	submitForm() {
		 
		this.loading = true;

		this.authService.confirmEmailAndSetPassword(this.model).subscribe((response:any) => {
			this.loading = false;
			if (response.isSuccess) {
				Swal.fire({
					title: 'Success',
					text: 'Password set successfully',
					icon: 'success'
				}).then((result) => {
					this.router.navigate(['/login']);
				})
			} else {
				Swal.fire('Error!', response.errorMessage, 'error');
			}
			
		});
	}

}
