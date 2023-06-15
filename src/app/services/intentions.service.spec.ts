import { TestBed } from '@angular/core/testing';

import { IntentionsService } from './intentions.service';

describe('IntentionsService', () => {
  let service: IntentionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(IntentionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
