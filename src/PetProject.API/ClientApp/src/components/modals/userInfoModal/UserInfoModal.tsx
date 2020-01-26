import React, { Component } from 'react';
import { connect } from 'react-redux';
import { groupBy } from 'lodash';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import Radio from '@material-ui/core/Radio';
import RadioGroup from '@material-ui/core/RadioGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormControl from '@material-ui/core/FormControl';
import FormLabel from '@material-ui/core/FormLabel';
import Divider from '@material-ui/core/Divider';
import Checkbox from '@material-ui/core/Checkbox';
import FormGroup from '@material-ui/core/FormGroup';

import { AppState } from '../../../store/appState';
import { UserFeature } from '../../../store/userFeaturesList/state';
import { requestUserFeaturesListData } from '../../../store/userFeaturesList/actions';

interface UserInfoModalProps {
  userFeatures: UserFeature[];
  open: boolean;
  loadUserFeaturesList: () => void;
  handleClose: () => void;
  handleSuccess: () => void;
}

interface UserFeatureCheckbox {
  id: string;
  checked: boolean;
}

interface UserInfoModalState {
  userFeatures: UserFeatureCheckbox[];
}

class UserInfoModal extends Component<UserInfoModalProps, UserInfoModalState> {
  constructor(props: UserInfoModalProps) {
    super(props);
    this.state = {
      userFeatures: []
    };
  }

  componentDidMount() {
    this.props.loadUserFeaturesList();
  }

  componentDidUpdate() {
    if (this.state.userFeatures.length !== this.props.userFeatures.length) {
      const grouped = groupBy(this.props.userFeatures, f => f.category);
      const checkedItems = Object.keys(grouped).map(key => {
        return key !== 'Readiness' ? grouped[key][0].userFeatureId : -1;
      });
      const userFeatures = this.props.userFeatures.map(f => {
        return {
          id: f.userFeatureId,
          checked: checkedItems.includes(f.userFeatureId),
        }
      });
      this.state = {
        userFeatures: userFeatures
      };
    };
  }

  changeFeatureCheckbox = (id: string) => {
    const { userFeatures } = this.state;
    const feature = userFeatures.find(f => f.id === id);
    if (feature !== undefined) {
      const checked = !feature.checked;
      feature.checked = checked;
      this.setState({
        userFeatures: userFeatures
      });
    }
    // this.props.loadPetsFilteredList(this.state.features.filter(f => f.checked).map(f => f.id));
  }

  changeFeatureRadio = (category: string, id: string) => {
    const { userFeatures } = this.state;
    const categoryItems = this.props.userFeatures.filter(f => f.category === category).map(f => f.userFeatureId);
    userFeatures.filter(f => categoryItems.includes(f.id)).forEach(f => f.checked = false);
    const feature = userFeatures.find(f => f.id === id);
    if (feature !== undefined) {
      const checked = !feature.checked;
      feature.checked = checked;
      this.setState({
        userFeatures: userFeatures
      });
    }
    // this.props.loadPetsFilteredList(this.state.features.filter(f => f.checked).map(f => f.id));
  }

  render() {
    const { userFeatures, open, handleClose, handleSuccess } = this.props;
    const grouped = groupBy(userFeatures, f => f.category);

    return (
      <div>
        <Dialog open={open} aria-labelledby="form-dialog-title">
          <DialogTitle id="form-dialog-title">Расскажите о себе</DialogTitle>
          <DialogContent>
            <FormControl component="fieldset">
              {Object.keys(grouped).map(key => {
                if (key === 'Readiness') {
                  return (
                    <>
                      <FormLabel component="legend">{key}</FormLabel>
                      <FormGroup row>
                        {grouped[key].map(f => {
                          const checked = this.state.userFeatures.find(feature => f.userFeatureId == feature.id)?.checked;
                          return (
                            <FormControlLabel key={`${f.userFeatureId}_${f.characteristic}`}
                              control={
                                <Checkbox
                                  onChange={() => this.changeFeatureCheckbox(f.userFeatureId)}
                                  key={`${f.userFeatureId}_${f.category}`}
                                  value={checked}
                                  color="primary"
                                />
                              }
                              label={f.characteristic}
                            />);
                        })}
                      </FormGroup>
                    </>
                  );
                }
                return (
                  <>
                    <FormLabel component="legend">{key}</FormLabel>
                    <RadioGroup defaultValue={grouped[key][0].characteristic} aria-label={key} row>
                      {grouped[key].map(f => {
                        return (
                          <FormControlLabel value={f.characteristic} key={`${f.userFeatureId}_${f.characteristic}`}
                            control={
                              <Radio
                                onChange={() => this.changeFeatureRadio(f.category, f.userFeatureId)}
                                key={`${f.userFeatureId}_${f.category}`}
                                color="primary"
                              />
                            }
                            label={f.characteristic}
                          />);
                      })}
                    </RadioGroup>
                  </>
                );
              })}
            </FormControl>
          </DialogContent>
          <DialogActions>
            <Button onClick={handleClose} color="secondary">
              Отмена
            </Button>
            <Button onClick={handleSuccess} color="primary">
              Готово
            </Button>
          </DialogActions>
        </Dialog>
      </div>
    );
  }
}

export const UserInfoModalConnected = connect(
  (appState: AppState) => ({
    userFeatures: appState.userFeatures.data
  }),
  {
    loadUserFeaturesList: requestUserFeaturesListData
  }
)(UserInfoModal);