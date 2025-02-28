import { TestBed } from '@angular/core/testing';

import { LoginComponent } from './login.component';

describe('LoginComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginComponent]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(LoginComponent);
    const login = fixture.componentInstance;
    expect(login).toBeTruthy();
  });

  it(`should have the 'crud-play - LogIn' title`, () => {
    const fixture = TestBed.createComponent(LoginComponent);
    const login = fixture.componentInstance;
    expect(login.title).toEqual('crud-play - LogIn');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(LoginComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('crud-play - LogIn');
  });
});
