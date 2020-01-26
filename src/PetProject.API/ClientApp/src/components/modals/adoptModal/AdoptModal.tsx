import React, { Component } from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import { Typography, Theme, createStyles, WithStyles, withStyles, Grid, Divider } from '@material-ui/core';

const styles = (theme: Theme) => createStyles({  
  root: {
    flexGrow: 1
  },
  image: {
    width: 300,
    height: 200,
  },
  img: {
    margin: 'auto',
    display: 'block',
    maxWidth: '100%',
    maxHeight: '100%',
  },
  divider: {
    margin: '8px 0'
  }
});

interface AdoptModalProps extends WithStyles<typeof styles> {
    open: boolean;
    handleClose: () => void;
    handleSuccess: () => void;
}

interface AdoptModalState {
    name: string;
    description: string;
    imagePath: string;
    descriptionFeatures: string[];
    ageFeatures: string[];
    furFeatures: string[];
}

export const AdoptModalStyled = withStyles(styles)(
  class AdoptModal extends Component<AdoptModalProps, AdoptModalState> {
    constructor(props: AdoptModalProps){
        super(props);
        this.state = {
            name: "",
            description: "",
            imagePath: "",
            descriptionFeatures: [],
            ageFeatures: [],
            furFeatures: []
        };
    }

    render() {
        const { open, handleClose, handleSuccess, classes } = this.props;

        return (
              <Dialog 
                  open={open} 
                  aria-labelledby="form-dialog-title"
                  fullWidth={true}
                  maxWidth = {'md'}>
                <DialogTitle id="form-dialog-title">О питомце</DialogTitle>
                <DialogContent>
                  <div className={classes.root}>
                    <Grid container spacing={3}>
                      <Grid item xs>
                        <div className={classes.image}>
                          <img className={classes.img} alt="complex" src="images/barsik.jpg" />
                        </div>
                      </Grid>
                      <Grid item xs>
                        <Typography variant={"h5"}>Барсик</Typography>
                        <Divider className={classes.divider} />
                        <Typography>Сладкий бушарик с блестящей шерсткой ищет любящих хозяев. Приучен к лотку, ловит мышей. Лащится, спит с вами в кровати, поет песни.</Typography>
                      </Grid>
                      <Grid item xs>                      
                        <Typography variant={"h6"}>Кто?</Typography>
                        <Typography>Кот</Typography>
                        <Typography>Мальчик</Typography>
                        <Divider className={classes.divider} />
                        <Typography variant={"h6"}>Возраст</Typography>
                        <Typography>До 3 лет</Typography>
                        <Divider className={classes.divider} />
                        <Typography variant={"h6"}>Шуба</Typography>
                        <Typography>Короткая</Typography>
                        <Typography>Цветная</Typography>
                      </Grid>
                    </Grid>
                  </div>
                </DialogContent>
                <DialogActions>
                  <Button onClick={handleClose} color="secondary">
                    Отмена
                  </Button>
                  <Button onClick={handleSuccess} color="primary">
                    Приютить!
                  </Button>
                </DialogActions>
              </Dialog>
          );
    }
  }
);