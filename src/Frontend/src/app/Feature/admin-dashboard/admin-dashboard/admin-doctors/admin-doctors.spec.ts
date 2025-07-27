import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminDoctors } from './admin-doctors';

describe('AdminDoctors', () => {
  let component: AdminDoctors;
  let fixture: ComponentFixture<AdminDoctors>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminDoctors]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminDoctors);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
