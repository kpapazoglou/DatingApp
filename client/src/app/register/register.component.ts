import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
 // @Input() usersFromHomeComponent: any; //parent to child communication 
  @Output() cancelRegister = new EventEmitter(); //child to parent communication
  model: any = {}

  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
  
  }

  register(){
    this.accountService.register(this.model).subscribe({
      next: () => {
        this.cancel();
      },
      error: error =>{
        this.toastr.error(error.error)
        console.log(error)
      } 
         
    })

  }

  cancel(){
    this.cancelRegister.emit(false)
  }

}
