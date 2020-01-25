import React, { Component } from 'react';
import { AppState } from '../../../store/appState';
import { connect } from 'react-redux';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import { sendLoginRequest } from '../../../store/loginForm/actions';

interface LoginModalProps {
    handleCancel: () => void;
    handleLogin: () => void;
}

interface LoginModalState {
    open: boolean;
    phone: string;
    password: string;
}

export class LoginModal extends Component<LoginModalProps, LoginModalState> {
    constructor(props: LoginModalProps){
        super(props);
        this.state = {
            open: true,
            phone: '',
            password: ''
        };
    }

    setInput = (fieldName: string, newValue: string) => {
        this.setState({...this.state, [fieldName]: newValue});
    }
    
    render() {
        const { handleCancel, handleLogin } = this.props;
        const { open, phone, password } = this.state;
        return (
            <div>
              <Dialog open={open} onClose={handleCancel} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">Мы вас ждали!</DialogTitle>
                <DialogContent>
                  <TextField
                    autoFocus
                    margin="dense"
                    id="phoneNumber"
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
                  <Button onClick={handleLogin} color="primary">
                    Войти
                  </Button>
                </DialogActions>
              </Dialog>
            </div>
          );
    }
}
