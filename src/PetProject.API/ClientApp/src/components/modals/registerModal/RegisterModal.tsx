import React, { Component } from 'react';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';

interface RegisterModalProps {
    open: boolean;
    handleCancel: () => void;
    handleRegister: () => void;
}

interface RegisterModalState {
    forename: string;
    surname: string;
    phone: string;
    password: string;
}

export class RegisterModal extends Component<RegisterModalProps, RegisterModalState> {
    constructor(props: RegisterModalProps){
        super(props);
        this.state = {
            forename: '',
            surname: '',
            phone: '',
            password: ''
        };
    }

    setInput = (fieldName: string, newValue: string) => {
        this.setState({...this.state, [fieldName]: newValue});
    }

    render() {
        const { open, handleCancel, handleRegister } = this.props;
        const { forename, surname, phone, password } = this.state;
        return (
            <div>
              <Dialog open={open} onClose={handleCancel} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">Добро пожаловать!</DialogTitle>
                <DialogContent>
                  <TextField
                    autoFocus
                    margin="dense"
                    id="forename"
                    label="Имя"
                    fullWidth
                    value={forename}
                    onChange={(e) => this.setInput('forename', e.target.value)}
                  />
                  <TextField
                    margin="dense"
                    id="surname"
                    label="Фамилия"
                    fullWidth
                    value={surname}
                    onChange={(e) => this.setInput('surname', e.target.value)}
                  />
                  <TextField
                    margin="dense"
                    id="phone"
                    label="Телефон"
                    fullWidth
                    value={phone}
                    onChange={(e) => this.setInput('phone', e.target.value)}
                  />
                  <TextField
                    margin="dense"
                    id="password"
                    label="Пароль"
                    type="password"
                    fullWidth
                    value={password}
                    onChange={(e) => this.setInput('password', e.target.value)}
                  />
                </DialogContent>
                <DialogActions>
                  <Button onClick={handleCancel} color="secondary">
                    Отмена
                  </Button>
                  <Button onClick={handleRegister} color="primary">
                    Регистрация
                  </Button>
                </DialogActions>
              </Dialog>
            </div>
          );
    }
  }
