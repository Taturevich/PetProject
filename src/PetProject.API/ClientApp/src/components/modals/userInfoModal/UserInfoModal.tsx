import React, { Component } from 'react';
import { connect } from 'react-redux';
import { useSelector, useDispatch } from 'react-redux'
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import Radio from '@material-ui/core/Radio';
import RadioGroup from '@material-ui/core/RadioGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import FormControl from '@material-ui/core/FormControl';
import FormLabel from '@material-ui/core/FormLabel';
import Divider from '@material-ui/core/Divider';
import Checkbox from '@material-ui/core/Checkbox';
import { AppState } from '../../../store/appState';

interface UserInfoModalProps {
    open: boolean;
    handleOk: () => void;
    handleChange: () => void;
}

interface UserInfoModalState {
    accommodation: number;
    isRented: boolean;
    experience: number;
    additionalOptions: number[];
}

export class UserInfoModal extends Component<UserInfoModalProps, UserInfoModalState> {
    constructor(props: UserInfoModalProps){
        super(props);
        this.state = {
            accommodation: 0,
            isRented: true,
            experience: 0,
            additionalOptions: []
        };
    }

    render() {
        const { open, handleOk } = this.props;
        const { accommodation, experience, additionalOptions } = this.state;
        return (
            <div>
              <Dialog open={open} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">Расскажите о себе</DialogTitle>
                <DialogContent>
                  <FormControl>
                    <FormLabel component="legend" >Жилье</FormLabel>
                    <RadioGroup aria-label="Жилье" name="accommodation" row>
                      <FormControlLabel
                        value="0"
                        control={<Radio color="primary" />}
                        checked={true}
                        labelPlacement="end"
                        label="Частный дом"/>
                      <FormControlLabel
                        value="1"
                        control={<Radio color="primary" />}
                        labelPlacement="end"
                        label="Квартира"/>
                      <FormControlLabel
                        value="2"
                        control={<Radio color="primary" />}
                        labelPlacement="end"
                        label="Общежитие"/>
                    </RadioGroup>
                    <RadioGroup aria-label="Жилье" name="isRented" row>
                      <FormControlLabel
                        value="0"
                        control={<Radio color="primary" />}
                        labelPlacement="end"
                        label="Собственность"/>
                      <FormControlLabel
                        value="1"
                        control={<Radio color="primary" />}
                        labelPlacement="end"
                        label="Аренда"/>
                    </RadioGroup>
                  </FormControl>
                  <Divider/>
                  <FormControl component="fieldset">
                    <FormLabel component="legend" >Опыт</FormLabel>
                    <RadioGroup aria-label="Опыт" name="experience" row>
                      <FormControlLabel
                        value="0"
                        control={<Radio color="primary" value='0'/>}
                        checked={true}
                        labelPlacement="end"
                        label="Никогда не было питомца"/>
                      <FormControlLabel
                        value="1"
                        control={<Radio color="primary" value='1'/>}
                        labelPlacement="end"
                        label="Был питомец"/>
                      <FormControlLabel
                        value="2"
                        control={<Radio color="primary" value='2'/>}
                        labelPlacement="end"
                        label="Питомец есть сейчас"/>
                    </RadioGroup>
                  </FormControl>
                  <Divider/>
                  <FormControl component="fieldset">
                    <FormLabel component="legend" >Готовы?</FormLabel>
                    <RadioGroup aria-label="Опыт" name="experience" row>
                      <FormControlLabel
                        value="0"
                        control={<Checkbox value="0" />}
                        labelPlacement="end"
                        label="Никогда не было питомца"/>
                      <FormControlLabel
                        value="1"
                        control={<Checkbox value="1" />}
                        labelPlacement="end"
                        label="Был питомец"/>
                      <FormControlLabel
                        value="2"
                        control={<Checkbox value="2" />}
                        labelPlacement="end"
                        label="Питомец есть сейчас"/>
                    </RadioGroup>
                  </FormControl>
                </DialogContent>
                <DialogActions>
                  <Button onClick={handleOk} color="primary">
                    Готово!
                  </Button>
                </DialogActions>
              </Dialog>
            </div>
          );
    }
  }

//   export const UserInfoModalConnected = connect(
//     (appState: AppState) => ({
//         pets: appState.pets.data,
//         features: appState.features.data
//     }),
//     {
//         loadPetsList: requestPetsListData,
//         loadFeaturesList: requestFeaturesListData,
//         loadPetsFilteredList: requestPetsListFilteredData
//     }
// )(PetSearchPageStyled);