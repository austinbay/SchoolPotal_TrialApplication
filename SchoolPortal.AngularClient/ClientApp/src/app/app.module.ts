import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DxDataGridModule, DxButtonModule, DxDateBoxModule } from 'devextreme-angular';
import { BlockUIModule } from 'ng-block-ui';


import { AppRoutingModule } from './app-routing.module';

// Layout
import { FooterOnlyLayoutComponent } from './_component/layout/footeronlylayout/footeronlylayout.component';
import { AppLayoutComponent } from './_component/layout/applayout/applayout.component';

// Secure Component
import { DashboardComponent } from './_component/dashboard/dashboard.component';

//Public Component
import { LoginComponent } from './_component/login/login.component';
import { SiteLayoutComponent } from './_component/layout/sitelayout/sitelayout.component';
import { AppComponent } from './app.component';

// Common
import { AuthGuard } from './_guards/auth.guard';
import { AppHeaderComponent } from './_component/layout/appheader/appheader.component';
import { SiteFooterComponent } from './_component/layout/sitefooter/sitefooter.component';
import { SiteHeaderComponent } from './_component/layout/siteheader/siteheader.component';
import { RegisterComponent } from './_component/register/register.component';
import { ConfirmemailComponent } from './_component/confirmemail/confirmemail.component';
import { ErrorInterceptor } from './_helpers/error.interceptor';
import { JwtInterceptor } from './_helpers/jwt.interceptor';
import { GlobalErrorHandler } from './global-error-handler';
import { StudentrecordComponent } from './_component/studentrecord/studentrecord.component';
import { StudentrecordAddComponent } from './_component/studentrecord/studentrecord-add.component';
import { StudentrecordEditComponent } from './_component/studentrecord/studentrecord-edit.component';
import { MustMatchDirective } from './_helpers/MustMatchDirective';


@NgModule({
	declarations: [
		AppComponent,
		DashboardComponent,
		LoginComponent,
		SiteLayoutComponent,
		AppHeaderComponent,
		SiteFooterComponent,
		SiteHeaderComponent,
		FooterOnlyLayoutComponent,
		AppLayoutComponent,
		RegisterComponent,
		ConfirmemailComponent,
		StudentrecordComponent,
		StudentrecordAddComponent,
		StudentrecordEditComponent,
		MustMatchDirective
	
	],
	imports: [
		BrowserModule,
		FormsModule,
		ReactiveFormsModule,
		HttpClientModule,
		AppRoutingModule,
		DxDataGridModule,
		DxButtonModule,
		DxDateBoxModule,
		BlockUIModule.forRoot()
	],
	providers: [
		AuthGuard,
		{ provide: ErrorHandler, useClass: GlobalErrorHandler },
		{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
		{ provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },

	],
	bootstrap: [AppComponent]
})
export class AppModule { }
