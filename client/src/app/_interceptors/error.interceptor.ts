import { Injectable, KeyValueDiffers } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, concatWith } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
//We're going to inject the router, so we're going to say private router and router so that we can redirect
//the user if we need to, depending on the error that we get back from the API,
  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error){
          switch (error.status){
            case 400: //1st error is the HttpResponse ,2nd error is the error object , 3rd is the errors  because//that is what is coming back or that's what our API server is sending back to us. .error.error.errors --> an array of objects
              if(error.error.errors){
                const modelStateErrors = [];
                for (const key in error.error.errors){
                  if(error.error.errors[key]){
                    modelStateErrors.push(error.error.errors[key])
                  }
                }
                throw modelStateErrors.flat(); //.falt flaten the 2 arrays to a single one , 
              } else {
                this.toastr.error(error.error, error.status.toString())
                }
                break;
              case 401:
                this.toastr.error('Unauthorized', error.status.toString())
                break;
              case 404:
                this.router.navigateByUrl('/not-found');
                break;
              case 500:
                const navigationExtras: NavigationExtras = {state: {error: error.error}};
                this.router.navigateByUrl('/server-error', navigationExtras);
                break;
              default:
                this.toastr.error('something unexpected went wrong');
                console.log(error);
                break;
          }
        }
        throw error;
      })
    )
  }
}
