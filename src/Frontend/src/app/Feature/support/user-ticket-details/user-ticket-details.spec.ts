import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserTicketDetails } from './user-ticket-details';

describe('UserTicketDetails', () => {
  let component: UserTicketDetails;
  let fixture: ComponentFixture<UserTicketDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserTicketDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserTicketDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
