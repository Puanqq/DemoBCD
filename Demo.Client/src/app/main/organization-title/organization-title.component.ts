import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { OrganizationServiceProxy, PaginationInputDto } from '@shared/service-proxies/service-proxies';
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

	pagination = null;

	constructor(
		private _organizationServiceProxy:OrganizationServiceProxy,
		private readonly _md: BsModalService,
		private _fb: FormBuilder,
	) {
	}

	ngOnInit() {
		this.resetPagination();
		this.getAll();
	}

	private resetPagination(){
		this.pagination = {
			items: [],
			totalCount: 0,
			skipCount : 0,
			maxCountResult: 2
		};
	
	}

	private getAll(){
		let input = PaginationInputDto.fromJS(this.pagination);
		this._organizationServiceProxy.getAll(input).subscribe(res => {
			this.pagination.items = res.items;
			this.pagination.totalCount = res.totalCount;
			_.each(this.pagination.items, item => {
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
			this.getAll();
		});
	}
}
