import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Injector, OnInit } from '@angular/core';
import { Form, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';
import { OrganizationInputDto, OrganizationServiceProxy, UpdateTitleUserOrganizationDto, UserOrganizationInputDto, UserOrganizationServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalService } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { BsModalRef } from "ngx-bootstrap";
import * as _ from 'lodash';

@Component({
	templateUrl: './updateTitleUserOrganization.component.html',
	styleUrls: [
		'./updateTitleUserOrganization.component.less'
	],
})
export class UpdateTitleUserOrganizationComponent implements OnInit {

	title;
	listTile = [];
	id;
	titleId;
	form:FormGroup;

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
		this.form = this._fb.group({
			'id': [this.id,[Validators.required]],
			'titleId': [this.titleId, [Validators.required]],
		});
	}


	save(){
		if(!this.form.valid) return;
		
		let input = UpdateTitleUserOrganizationDto.fromJS(this.form.value);

		this._userOrganizationServiceProxy.updateTitleUserOrganizationById(input).subscribe(res => {
			toastr.success("Save successfully");
			this.onSave.next();
			this._modalRef.hide();
		});
	}

	delete(){
		this._userOrganizationServiceProxy.delete(this.form.get('id').value).subscribe(res => {
			toastr.success("Delete successfully");
			this.onSave.next();
			this._modalRef.hide();
		})
	}
}
