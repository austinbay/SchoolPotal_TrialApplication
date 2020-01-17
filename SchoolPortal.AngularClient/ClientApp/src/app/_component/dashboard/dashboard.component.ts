

// dashboard.component.ts
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../_services/auth.service';

@Component({
	selector: 'app-dashboard',
	templateUrl: './dashboard.component.html',
	styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
	model: any = {};

	dataFromServer: any = [];

	constructor(private authService: AuthService) {

	}

	ngOnInit() {
		this.getSomePrivateStuff();
	}

	getSomePrivateStuff() {
		
		this.authService.getData().subscribe(response => {
			this.dataFromServer = response;
		}, error => {
			this.authService.logout();
		});
	}

	logout() {
		this.authService.logout();
	}

}
