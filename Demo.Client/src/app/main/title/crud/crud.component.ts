import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Injector, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';
import { OrganizationInputDto, OrganizationServiceProxy, TitleServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalService } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { BsModalRef } from "ngx-bootstrap";

@Component({
	templateUrl: './crud.component.html',
	styleUrls: [
		'./crud.component.less'
	],
})
export class CrudTitleComponent implements OnInit {

	form:FormGroup;
	title;
	isNew;
	id;

	_modalRef:BsModalRef;
    onSave:Subject<any> = new Subject<any>();
    onDelete:Subject<any> = new Subject<any>();
    onClose:Subject<any> = new Subject<any>();
	
	constructor(
		injector: Injector,
		private _titleServiceProxy:TitleServiceProxy,
		private _md:BsModalService,
		private _fb:FormBuilder,
	) {
		this._modalRef = injector.get(BsModalRef)
	}

	ngOnInit() {
		if(this.isNew){
			this.form = this._fb.group({
				'id': [AppConsts.IdZero],
				'codeValue': ['', [Validators.required]],
				'name': ['', [Validators.required]],
			});
		}
		else{
			this._titleServiceProxy.get(this.id).subscribe(res => {
				this.form = this._fb.group({
					'id': [this.id],
					'codeValue': [res.codeValue, [Validators.required]],
					'name': [res.name, [Validators.required]],
				});
			})
		}
	}


	save(){
		if(!this.form.valid) return;
		
		let input = OrganizationInputDto.fromJS(this.form.value);


		if(this.isNew){
			this._titleServiceProxy.create(input).subscribe(res => {
				toastr.success("Save successfully");
				this.onSave.next();
				this._modalRef.hide();
			});
		}
		else {
			this._titleServiceProxy.update(input).subscribe(res => {
				toastr.success("Save successfully");
				this.onSave.next();
				this._modalRef.hide();
			});
		}
	}

	delete(){
		this._titleServiceProxy.delete(this.form.get('id').value).subscribe(res => {
			toastr.success("Delete successfully");
			this.onDelete.next();
			this._modalRef.hide();
		})
	}
}
