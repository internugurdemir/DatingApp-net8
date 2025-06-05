import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccountService } from '../_services/account.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  /*
  "req" is an immutable object cannot be changed after it's created.
In Angular, HttpRequest is immutable — you can't change its headers or body directly.
Instead, you must use .clone() to create a new request with the changes.
   */
  const accountService = inject(AccountService);

  if (accountService.currentUser()) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accountService.currentUser()?.token}`
      }
    })
  }

  return next(req);
};