import { Component, OnInit } from '@angular/core';
import { StudentRecordService } from '../../_services/studentrecord.service';
import { environment } from 'src/environments/environment';
import * as AspNetData from "devextreme-aspnet-data-nojquery";
import { AuthService } from '../../_services/auth.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { confirm } from 'devextreme/ui/dialog';
import { BlockUI, NgBlockUI } from 'ng-block-ui';


@Component({
  selector: 'app-studentrecord',
  templateUrl: './studentrecord.component.html',
  styleUrls: ['./studentrecord.component.css']
})
export class StudentrecordComponent implements OnInit {

	// Decorator wires up blockUI instance
	@BlockUI() blockUI: NgBlockUI;
	items: any = [];
	dataSource: any;
	apiUrl: string;
	formTitle: string;


	constructor(private studentRecordService: StudentRecordService, private authService: AuthService,
		private router: Router) {

		this.apiUrl = environment.basePath + 'api/StudentRecord/';
		this.formTitle = 'Manage Students';
	}

	ngOnInit() {

		this.getStudentRecords();	
	}

	getStudentRecords() {
		this.dataSource = AspNetData.createStore({
			key: "id",
			loadUrl: this.apiUrl + "LoadAll",
			onBeforeSend: function (method, ajaxOptions) {
				ajaxOptions.xhrFields = {
					withCredentials: false,
					contentType: 'application/json',

				};
			}
		});
	
	}
	cancel() {
		this.router.navigate(['/studentrecord']);
	}
	editItem(item) {	 
		this.router.navigate(['/editstudent', item.data.id ]);
	}
	deleteItem(item) {
		
		let fName = item.data.lastName + ' ' + item.data.firstName;
		Swal.fire({
			title: 'Confirm Delete',
			text: 'Do you want to delete ' + fName + '?',
			icon: 'warning',
			showCancelButton: true,
			confirmButtonText: 'Yes',
			cancelButtonText: 'No',
			
		}).then((result) => {
			if (result.value) {
				this.blockUI.start('Deleting...');
				this.studentRecordService.deleteItem(item.data.id).subscribe((response: any) => {					
					this.blockUI.stop();// Stop blocking					
					if (response.isSuccess) {
						  this.getStudentRecords();
					} else {
						Swal.fire('Error!', response.errorMessage, 'error');
					}

				});
				
			}
		})
	}
 
}
