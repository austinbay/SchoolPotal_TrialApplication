import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { RouterModule, Routes} from '@angular/router';

import { AuthGuard } from './_guards/auth.guard';
import { FooterOnlyLayoutComponent } from './_component/layout/footeronlylayout/footeronlylayout.component';
import { FOOTERONLYLAYOUT_ROUTES } from './_component/layout/footeronlylayout/footeronlylayout.Routing';
import { AppLayoutComponent } from './_component/layout/applayout/applayout.component';
import { APPLAYOUT_ROUTES } from './_component/layout/applayout/APPlayout.Routing';


/**
 * Route constant 
 */
const routes: Routes = [
	{ path: '', redirectTo: 'login', pathMatch: 'full' },
	{ path: '', component: FooterOnlyLayoutComponent, children: FOOTERONLYLAYOUT_ROUTES },
	{ path: '', component: AppLayoutComponent, canActivate: [AuthGuard], children: APPLAYOUT_ROUTES },
	{ path: '**', redirectTo: 'login' }
];

/**
 * App routing module
 */
@NgModule({
	imports: [
		RouterModule.forRoot(routes)
	],
	exports: [RouterModule]
})
export class AppRoutingModule { }
