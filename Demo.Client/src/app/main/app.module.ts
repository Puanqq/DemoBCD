import { NgModule, Injector, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JsonpModule } from '@angular/http';
import { HttpClientModule, HttpResponse } from '@angular/common/http';
import { NgxErrorsModule } from "@hackages/ngxerrors";
import { ModalModule } from 'ngx-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { AbpModule } from '@abp/abp.module';

import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { LoginComponent } from './login/login.component';
import { TokenService } from 'abp-ng2-module/dist/src/auth/token.service';
import { RegisterComponent } from './register/register.component';
import { OrganizationComponent } from './organization/organization.component';
import { CrudOrganizationComponent } from './organization/crud/crud.component';
import { CrudTitleComponent } from './title/crud/crud.component';
import { TitleComponent } from './title/title.component';
import { CrudOrganizationTitleComponent } from './organization-title/crud/crud.component';
import { OrganizationTitleComponent } from './organization-title/organization-title.component';
import { AddUserOrganizationComponent } from './userOrganization/addUserOrganization/addUserOrganization.component';
import { UserOrganizationComponent } from './userOrganization/userOrganization.component';
import { UpdateTitleUserOrganizationComponent } from './userOrganization/updateTitleUserOrganization/updateTitleUserOrganization.component';
import { PaginationComponent } from './pagination/pagination.component';


@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        RegisterComponent,
        OrganizationComponent,
        CrudOrganizationComponent,
        TitleComponent,
        CrudTitleComponent,
        CrudOrganizationTitleComponent,
        OrganizationTitleComponent,
        UserOrganizationComponent,
        AddUserOrganizationComponent,
        UpdateTitleUserOrganizationComponent,
        PaginationComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        NgxErrorsModule,
        JsonpModule,
        ModalModule.forRoot(),
        AbpModule,
        AppRoutingModule,
        ServiceProxyModule,
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
    providers: [
        TokenService
    ],
    entryComponents: [
        CrudOrganizationComponent,
        CrudTitleComponent,
        CrudOrganizationTitleComponent,
        AddUserOrganizationComponent,
        UpdateTitleUserOrganizationComponent
    ]
})
export class AppModule {
    
    constructor(){}
}
