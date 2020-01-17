import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { StudentRecordService } from '../../_services/studentrecord.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import Swal from 'sweetalert2'






@Component({
  selector: 'app-studentrecord-form',
  templateUrl: './studentrecord-form.component.html',
})
export class StudentrecordAddComponent implements OnInit {

	@BlockUI() blockUI: NgBlockUI;

	model: any = {};
	
	formTitle: string;
	

	constructor(private studentRecordService: StudentRecordService, private router: Router) {

		this.formTitle = 'Add Student';
		 
	}

	ngOnInit() {

	}

	onSubmit() {

		this.blockUI.start('Submitting ...');
		this.studentRecordService.addItem(this.model).subscribe((response:any) => {
			this.blockUI.stop();
			if (response.isSuccess) {
				Swal.fire({
					title: 'Success',
					text: 'Submitted successfully',
					icon: 'success'
				}).then((result) => {
					this.router.navigate(['/studentrecord']);
				})
			} else {
				Swal.fire('Error!', response.errorMessage, 'error');
			}
		});

	}

	cancel() {
		this.router.navigate(['/studentrecord']);
	}

	
}




