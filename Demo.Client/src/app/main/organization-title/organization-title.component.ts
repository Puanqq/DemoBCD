import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { OrganizationServiceProxy } from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';
import { BsModalService } from 'ngx-bootstrap';
import { CrudOrganizationTitleComponent } from './crud/crud.component';

@Component({
	templateUrl: './organization-title.component.html',
	styleUrls: [
		'./organization-title.component.less'
	],
})

export class OrganizationTitleComponent implements OnInit {

	list:any[] = [];

	constructor(
		private _organizationServiceProxy:OrganizationServiceProxy,
		private readonly _md: BsModalService,
		private _fb: FormBuilder,
	) {
	}

	ngOnInit() {
		this.getList();
	}

	private getList(){
		this._organizationServiceProxy.getList().subscribe(res => {
			this.list = res;
			_.each(this.list, item => {
				item.listTitle = JSON.parse(item.titles);
			})
		})
	}

	openDialog(id?){
    
		var modal = this._md.show(CrudOrganizationTitleComponent, {
		  keyboard: false,
		  class: 'md',
		  initialState: {
			title: id ? 'Cập nhật phòng ban' : 'Thêm phòng ban',
			id: id,
			isNew: id ? false : true
		  }
		});
	
		modal.content.onSave.subscribe(res => {
			this.getList();
		});
	}
}
