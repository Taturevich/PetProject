import { createStore, applyMiddleware, Action } from 'redux';
import thunk, { ThunkAction } from "redux-thunk";
import { composeWithDevTools } from 'redux-devtools-extension';

import { rootReducer } from './rootReducer';
import { PetsListState } from './petsList/state';
import { FeaturesListState } from './featuresList/state';
import { UserInfoListState } from './userInfo/state';

export interface AppState {
  pets: PetsListState;
  features: FeaturesListState;
  usersInfo: UserInfoListState;
}

const initialState: AppState = {
  pets: {
    data: []
  },
  features: {
    data: []
  },
  usersInfo: {
    data: []
  }
};

export const store = createStore(rootReducer, initialState, composeWithDevTools(applyMiddleware(thunk)));

export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  typeof store,
  null,
  Action<string>
>;
