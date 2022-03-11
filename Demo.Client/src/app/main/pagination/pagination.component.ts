import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
	templateUrl: './pagination.component.html',
	selector: 'app-pagination',
	styleUrls: [
		'./pagination.component.less'
	],
})
export class PaginationComponent implements OnInit {

	@Input('pagination') pagination;
	@Output('onGetAll') onGetAll = new EventEmitter();


	constructor(
	) {
	}

	ngOnInit() {
	}

	getListPageination(){
		
		let listPageination = [];

		let totalCountPage = this.pagination.totalCount / this.pagination.maxCountResult;

		for(let i = 0; i < totalCountPage; i++){

			listPageination.push({
				id: i * this.pagination.maxCountResult,
				name: i + 1
			})
		}

		return listPageination;
	}

	selectPage(skipCount){
 
		if(this.pagination.skipCount == skipCount || skipCount < 0 || skipCount > this.pagination.totalCount) return;

		this.pagination.skipCount = skipCount;
		this.onGetAll.next();
	}

}
