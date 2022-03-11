import { Component, Injector, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { AuthenticateDto, AuthenticationServiceProxy, RegisterUserInputDto } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { UtilsService } from 'abp-ng2-module/dist/src/utils/utils.service';
import { AppConsts } from '@shared/AppConsts';
import { TokenService } from 'abp-ng2-module/dist/src/auth/token.service';

@Component({
	templateUrl: './register.component.html',
	styleUrls: [
		'./register.component.less'
	],
})
export class RegisterComponent implements OnInit {

	form: FormGroup;

	constructor(
		private _authenticationServiceProxy:AuthenticationServiceProxy,
		private _fb: FormBuilder,
	) {
	}

	ngOnInit() {
		this.form = this._fb.group({
			'userName': ['', [Validators.required, Validators.maxLength(255)]],
			'password': ['', [Validators.required, Validators.maxLength(32)]],
			'surname': ['', [Validators.required, Validators.maxLength(32)]],
			'name': ['', [Validators.required, Validators.maxLength(32)]],
		});
	}


	save(): void {

		if (this.form.invalid) return;

		let input = RegisterUserInputDto.fromJS(this.form.value);

		this._authenticationServiceProxy.register(input).subscribe(res => {
			toastr.success("Sign Up Success");
		}, err => {
			toastr.error(err);
		})

	}
}
