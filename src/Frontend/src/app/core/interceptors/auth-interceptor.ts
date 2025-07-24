import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token =
    sessionStorage.getItem('token') || localStorage.getItem('token');
  // console.log(':toke', token);
  if (token == null) return next(req);
  req = req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`,
    },
  });
  return next(req);
};
