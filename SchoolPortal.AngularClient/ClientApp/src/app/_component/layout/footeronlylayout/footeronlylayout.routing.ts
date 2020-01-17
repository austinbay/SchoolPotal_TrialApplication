



import { Routes } from '@angular/router';
import { LoginComponent } from '../../../_component/login/login.component';
import { RegisterComponent } from '../../../_component/register/register.component';
import { ConfirmemailComponent } from '../../../_component/confirmemail/confirmemail.component';


export const FOOTERONLYLAYOUT_ROUTES: Routes = [
	{ path: '', redirectTo: 'login', pathMatch: 'full' },
	{ path: 'login', component: LoginComponent },
	{ path: 'register', component: RegisterComponent },
	{ path: 'confirmemail/:userId/:code', component: ConfirmemailComponent }
];
