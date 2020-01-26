import React from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogTitle from '@material-ui/core/DialogTitle';

interface CareSuccessModalProps {
  open: boolean;
  petName: string;
  handleClose: () => void;
  handleSuccess: () => void;
}

export class CareSuccessModal extends React.Component<CareSuccessModalProps> {
  render() {
    const { open, petName, handleClose, handleSuccess } = this.props;
    return (
      <div>
        <Dialog open={open} onClose={handleClose} aria-labelledby="form-dialog-title">
          <DialogTitle id="form-dialog-title">Поздравляем! С Вами свяжутся по поводу {petName}</DialogTitle>
          <DialogActions>
            <Button onClick={handleSuccess} color="primary">
              Готово
            </Button>
          </DialogActions>
        </Dialog>
      </div>
    );
  }
}