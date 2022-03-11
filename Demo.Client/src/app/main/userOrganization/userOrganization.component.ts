import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';
import { OrganizationServiceProxy, PaginationInputDto, UserOrganizationServiceProxy } from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';
import { BsModalService } from 'ngx-bootstrap';
import { AddUserOrganizationComponent } from './addUserOrganization/addUserOrganization.component';
import { UpdateTitleUserOrganizationComponent } from './updateTitleUserOrganization/updateTitleUserOrganization.component';

@Component({
	templateUrl: './userOrganization.component.html',
	styleUrls: [
		'./userOrganization.component.less'
	],
})
export class UserOrganizationComponent implements OnInit {

	pagination = null;

	constructor(
		private _organizationServiceProxy:OrganizationServiceProxy,
		private _userOrganizationServiceProxy:UserOrganizationServiceProxy,
		private _md: BsModalService,
		private _fb: FormBuilder,
	) {
	}

	ngOnInit() {
		this.resetPagination();
		this.getAll();
	}

	addUserOrganization(organization){
		var modal = this._md.show(AddUserOrganizationComponent, {
			keyboard: false,
			class: 'md',
			initialState: {
				title: 'More staff',
				organization: _.cloneDeep(organization)
			}
		});
	
		modal.content.onSave.subscribe(res => {
			this.getAll();
		});
	}

	updateTitleUserOrganization(organization, userOrganization){
		var modal = this._md.show(UpdateTitleUserOrganizationComponent, {
			keyboard: false,
			class: 'md',
			initialState: {
				title: 'Update titles for employees',
				id: userOrganization.id,
				titleId: userOrganization.titleId,
				listTile: organization.titles ? JSON.parse(organization.titles) : []
			}
		});
	
		modal.content.onSave.subscribe(res => {
			this.getAll();
		});
	}


	getUserOrganization(index){

		if(this.pagination.items[index].userOrganizations) return;

		this._userOrganizationServiceProxy.getListUserOrganizationByOrganizationId(this.pagination.items[index].id).subscribe(res => {
			this.pagination.items[index].userOrganizations = res;
		})
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
		})
	}
}
