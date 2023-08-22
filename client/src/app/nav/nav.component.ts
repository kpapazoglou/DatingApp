import { Component, EnvironmentInjector, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}; //a new class names any' , to access this model model.username  , model.password


  constructor(public accountService: AccountService, private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    
  }

 

  login(){
    this.accountService.login(this.model).subscribe({
      next: _ => // "_ " ---> we are not using an argument for this particular method
        this.router.navigateByUrl('/members'),
       // error: error => this.toastr.error(error.error) //,,,the toastr message  is handled inside intercepto
      })
  
  }
  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/')
  }

}
