import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../_services/auth.service';


@Component({
	selector: 'app-main-layout',
  templateUrl: './applayout.component.html',
  styleUrls: ['./applayout.component.css']
})
export class AppLayoutComponent implements OnInit {

	userName: string;
	constructor(private authService: AuthService) {

	}

	ngOnInit() {
		this.userName = this.authService.getUserName();
	}

	logout() {
		this.authService.logout();
	}

}
