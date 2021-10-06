
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter<boolean>()
  registerForm:FormGroup;
  maxdate:Date;
  validationErrors: string[] = [];

  constructor(
    private accountService : AccountService,
    private fb: FormBuilder,
    private router:Router ) { }

  ngOnInit(): void {
    this.initializaForm();
    this.maxdate = new Date();
    this.maxdate.setFullYear(this.maxdate.getFullYear()-18);
  }

  initializaForm(){
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['',Validators.required],
      knownAs: ['',Validators.required],
      dateOfBirth: ['',Validators.required],
      city: ['',Validators.required],
      country: ['',Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });
    //this is necessary because if the password is changed after confirm password validation has succeded
    //the form continues to be valid.The we subscribe the form field validity any time the password changes
    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    });
  }

  matchValues(matchTo:string) : ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
      ? null
      : {isMatching:true}
    }
  }

  register(){
    this.accountService.register(this.registerForm.value).subscribe( response => {
        this.router.navigateByUrl('/members');
    },
    error => {
        this.validationErrors = error;
      });
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

}
