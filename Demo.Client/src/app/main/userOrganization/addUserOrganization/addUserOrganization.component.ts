import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Injector, OnInit } from '@angular/core';
import { Form, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';
import { OrganizationInputDto, OrganizationServiceProxy, UserOrganizationInputDto, UserOrganizationServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalService } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { BsModalRef } from "ngx-bootstrap";
import * as _ from 'lodash';

@Component({
	templateUrl: './addUserOrganization.component.html',
	styleUrls: [
		'./addUserOrganization.component.less'
	],
})
export class AddUserOrganizationComponent implements OnInit {

	form:FormGroup;
	title;
	organization;
	listTile = [];
	listUser:any[] = [];

	_modalRef:BsModalRef;
    onSave:Subject<any> = new Subject<any>();
    onDelete:Subject<any> = new Subject<any>();
    onClose:Subject<any> = new Subject<any>();
	
	constructor(
		injector: Injector,
		private _userOrganizationServiceProxy:UserOrganizationServiceProxy,
		private _md:BsModalService,
		private _fb:FormBuilder,
	) {
		this._modalRef = injector.get(BsModalRef)
	}

	ngOnInit() {
		this.getListNotUserDependencyOrganization();
		this.listTile = this.organization.titles ? JSON.parse(this.organization.titles) : [];
		this.form = this._fb.group({
			'id': [AppConsts.IdZero],
			'userId': [null, [Validators.required]],
			'titleId': [null, [Validators.required]],
			'organizationId': [this.organization.id, [Validators.required]],
		});
	}

	selectUser(id, index){
		this.form.get('userId').setValue(id);

		_.each(this.listUser, (item,indexE) => {
			
			if(index != indexE){
				item._isCheck = false;
			}
			else{
				item._isCheck = true;
			}

		})
	}

	selectTitle(id, index){
		this.form.get('titleId').setValue(id);

		_.each(this.listTile, (item,indexE) => {
			
			if(index != indexE){
				item._isCheck = false;
			}
			else{
				item._isCheck = true;
			}

		})
	}

	save(){
		if(!this.form.valid) return;
		
		let input = UserOrganizationInputDto.fromJS(this.form.value);

		this._userOrganizationServiceProxy.create(input).subscribe(res => {
			toastr.success("Save successfully");
			this.onSave.next();
			this._modalRef.hide();
		});
	}

	private getListNotUserDependencyOrganization(){
		this._userOrganizationServiceProxy.getListUserNotDependencyUserOrganization(this.organization.id).subscribe(res => {
			this.listUser = res;
		})
	}
}
