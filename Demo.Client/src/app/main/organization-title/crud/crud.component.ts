import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Injector, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';
import { OrganizationInputDto, OrganizationServiceProxy, TitleInputDto, TitleOrganizationInputDto, TitleServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalService } from 'ngx-bootstrap';
import { forkJoin, Subject } from 'rxjs';
import { BsModalRef } from "ngx-bootstrap";
import * as _ from 'lodash';

@Component({
	templateUrl: './crud.component.html',
	styleUrls: [
		'./crud.component.less'
	],
})
export class CrudOrganizationTitleComponent implements OnInit {

	form:FormGroup;
	title;
	id;

	data;
	list:any[] = [];

	_modalRef:BsModalRef;
    onSave:Subject<any> = new Subject<any>();
    onDelete:Subject<any> = new Subject<any>();
    onClose:Subject<any> = new Subject<any>();
	
	constructor(
		injector: Injector,
		private _organizationServiceProxy:OrganizationServiceProxy,
		private _titleServiceProxy:TitleServiceProxy,
		private _md:BsModalService,
		private _fb:FormBuilder,
	) {
		this._modalRef = injector.get(BsModalRef)
	}

	ngOnInit() {
		forkJoin([
			this._organizationServiceProxy.get(this.id),
			this._titleServiceProxy.getList()
		]).subscribe(result => {
			
			let titleOrganizations = JSON.parse(result[0].titles);

			this.list = result[1];

			_.each(this.list, item => {
				
				let found = _.find(titleOrganizations, f => {
					return f.id == item.id;
				})

				item._isCheck = found ? true : false;
			})

		})
	}

	save(){

		let input = new TitleOrganizationInputDto({
			id: this.id,
			listTitle: _.filter(this.list, item => {
				return item._isCheck;
			}).map(m => TitleInputDto.fromJS(m))
		});

		this._organizationServiceProxy.updateTitleOrganization(input).subscribe(res => {
			toastr.success("Lưu thành công");
			this.onSave.next();
			this._modalRef.hide();
		});

	}

	delete(){
		this._organizationServiceProxy.delete(this.form.get('id').value).subscribe(res => {
			toastr.success("Xóa thành công");
			this.onDelete.next();
			this._modalRef.hide();
		})
	}
}
