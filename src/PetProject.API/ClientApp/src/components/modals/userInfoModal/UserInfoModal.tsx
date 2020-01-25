import React, { Component } from 'react';
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

interface UserInfoModalProps {
    open: boolean;
    handleOk: () => void;
}

interface UserInfoModalState {
    accommodation: number;
    experience: number;
    additionalOptions: number[];
}

const flexContainer = {
  display: 'flex',
  flexDirection: 'row',
};

export class UserInfoModal extends Component<UserInfoModalProps, UserInfoModalState> {
    constructor(props: UserInfoModalProps){
        super(props);
        this.state = {
            accommodation: 0,
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
                  <FormControl component="fieldset">
                    <FormLabel component="legend">Жилье</FormLabel>
                    <RadioGroup aria-label="Жилье" name="accommodation" value={value} style={flexContainer}>
                      <FormControlLabel
                        value="0"
                        control={<Radio color="primary" />}
                        labelPlacement="start"
                        label="Частный дом"/>
                      <FormControlLabel
                        value="1"
                        control={<Radio color="primary" />}
                        labelPlacement="start"
                        label="Квартира"/>
                      <FormControlLabel
                        value="2"
                        control={<Radio color="primary" />}
                        labelPlacement="start"
                        label="Общежитие"/>
                    </RadioGroup>
                  </FormControl>
                </DialogContent>
                <DialogActions>
                  <Button onClick={handleOk} color="primary">
                    Отмена
                  </Button>
                </DialogActions>
              </Dialog>
            </div>
          );
    }
  }
