import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';
import { PaginationInputDto, TitleServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsModalService } from 'ngx-bootstrap';
import { CrudTitleComponent } from './crud/crud.component';

@Component({
	templateUrl: './title.component.html',
	styleUrls: [
		'./title.component.less'
	],
})
export class TitleComponent implements OnInit {

	pagination = null;

	constructor(
		private _titleServiceProxy:TitleServiceProxy,
		private _md: BsModalService,
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
		this._titleServiceProxy.getAll(input).subscribe(res => {
			this.pagination.items = res.items;
			this.pagination.totalCount = res.totalCount;
		})
	}

	openDialog(id?){
    
		var modal = this._md.show(CrudTitleComponent, {
		  keyboard: false,
		  class: 'md',
		  initialState: {
			title: id ? 'Cập nhật chức danh' : 'Thêm chức danh',
			id: id,
			isNew: id ? false : true
		  }
		});
	
		modal.content.onSave.subscribe(res => {
			this.getAll();
		});

		modal.content.onDelete.subscribe(res => {
			this.resetPagination();
			this.getAll();
		});
	}
}
