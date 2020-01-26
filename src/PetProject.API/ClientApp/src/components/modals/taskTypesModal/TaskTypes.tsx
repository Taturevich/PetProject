import React, { Component } from 'react';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import classes from '*.module.css';
import {FormLabel, FormControl, FormGroup, FormControlLabel, Checkbox, FormHelperText} from '@material-ui/core';

interface TaskTypesModalProps {
    open: boolean;
    handleCancel: () => void;
    handleSubmit: () => void;
}

interface TaskTypesModalState {
    name: string;
    description: string;
}

export class TaskTypesModal extends Component<TaskTypesModalProps, TaskTypesModalState> {
    constructor(props: TaskTypesModalProps){
        super(props);
        this.state = {
            name: '',
            description:'',
        };
    }

    setInput = (fieldName: string, newValue: string) => {
        this.setState({...this.state, [fieldName]: newValue});
    }

    render() {
        const { open, handleSubmit, handleCancel } = this.props;
        const { name, description } = this.state;
        return (
            <div>
              <Dialog open={open} onClose={handleCancel} aria-labelledby="form-dialog-title">
                <DialogTitle id="form-dialog-title">Я готова, я готов...</DialogTitle>
                <DialogContent>
                    <FormControl component="fieldset">
                        <FormGroup>
                            <FormControlLabel control={<Checkbox value="pr-agent" />} label="Стать PR агентом питомца"/>
                            <FormHelperText style={{ fontSize: '15px'}}>Испытайте свои таланты и отзывчивость ваших знакомых - возьмитесь за задачу приютить отыскать питомцу дом.
                                Срок на выполнение: 30 дней.</FormHelperText>
                            <FormControlLabel control={<Checkbox value="fee-pet" />} label="Покормить питомца" />
                            <FormHelperText style={{ fontSize: '15px'}}>Бездомным животным часто нужна мед. помощь - от стерилизации до травм от жестокого обращения.
                                Срок на выполнение: 14 дней.</FormHelperText>
                            <FormControlLabel control={<Checkbox value="heal-pet" />} label="Свозить питомца в вет. клинику" />
                            <FormHelperText style={{ fontSize: '15px'}}>Волонтёры содержат сотни животных, в этом задании всё просто - нужно купить животному корм на срок от недели.
                                Срок на выполнение: 7 дней.</FormHelperText>
                        </FormGroup>
                    </FormControl>
                </DialogContent>
                <DialogActions>
                  <Button onClick={handleSubmit} color="primary">
                    Найти подопечного
                  </Button>
                </DialogActions>
              </Dialog>
            </div>
          );
    }
  }
