import { createStore, applyMiddleware, Action } from 'redux';
import thunk, { ThunkAction } from "redux-thunk";
import { composeWithDevTools } from 'redux-devtools-extension';

import { rootReducer } from './rootReducer';
import { PetsListState } from './petsList/state';

export interface AppState {
  pets: PetsListState;
}

const initialState: AppState = {
  pets: {
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
