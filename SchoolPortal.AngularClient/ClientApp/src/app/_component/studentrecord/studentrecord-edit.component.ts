import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { StudentRecordService } from '../../_services/studentrecord.service';
import { StudentRecordModel } from '../../_models/studentrecord';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import Swal from 'sweetalert2';


@Component({
	selector: 'app-studentrecord-form',
	templateUrl: './studentrecord-form.component.html',
})
export class StudentrecordEditComponent implements OnInit {

	@BlockUI() blockUI: NgBlockUI;

	model: any = {};
	itemId: number;
	formTitle: string;

	existingItem: any;

	constructor(private studentRecordService: StudentRecordService, private formBuilder: FormBuilder,
		private avRoute: ActivatedRoute, private router: Router) {

		const idParam = 'id';
		
		this.formTitle = 'Edit Student';
	 
		if (this.avRoute.snapshot.params[idParam]) {
			this.itemId = this.avRoute.snapshot.params[idParam];
		}

	}

	ngOnInit() {

		if (this.itemId > 0) {
		 
			this.blockUI.start('Loading...');
			this.studentRecordService.getItem(this.itemId).subscribe((response: any) => {

				this.blockUI.stop();
				if (response.isSuccess) {
					this.model = response.data;
					//this.model.dateOfBirth = new Date(response.data.dateOfBirth);

				} else {
					Swal.fire('Error!', response.errorMessage, 'error');
				}

			});

		}
	}
	 
		 
	onSubmit() {

		this.blockUI.start('Submitting ...');
		this.studentRecordService.updateItem(this.model).subscribe((response: any) => {
			this.blockUI.stop();
			if (response.isSuccess) {
				Swal.fire({
					title: 'Success',
					text: 'Updated successfully',
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




