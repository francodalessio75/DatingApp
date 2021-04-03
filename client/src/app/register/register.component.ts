import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model:any = {};
  @Output() cancelRegister = new EventEmitter()

  constructor() { }

  ngOnInit(): void {
  }

  register( model:any){
    console.log(this.model);
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

}
