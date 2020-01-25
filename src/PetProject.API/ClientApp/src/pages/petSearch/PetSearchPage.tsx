import React from 'react';
import { Theme, createStyles, makeStyles } from '@material-ui/core/styles';
import GridList from '@material-ui/core/GridList';
import GridListTile from '@material-ui/core/GridListTile';
import GridListTileBar from '@material-ui/core/GridListTileBar';
import ListSubheader from '@material-ui/core/ListSubheader';
import IconButton from '@material-ui/core/IconButton';
import InfoIcon from '@material-ui/icons/Info';
import Button from '@material-ui/core/Button';

import petSearch from '../../static/images/petSearch.png';
import petHelp from '../../static/images/petHelp.png';
import petSaw from '../../static/images/petSaw.png';

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            display: 'flex',
            flexWrap: 'wrap',
            justifyContent: 'space-around',
            overflow: 'hidden',
            backgroundColor: theme.palette.background.paper,
        },
        gridList: {
            width: 500,
            height: 450,
        },
        icon: {
            color: 'rgba(255, 255, 255, 0.54)',
        },
    }),
);

const tileData = [
    {
        img: petSearch,
        title: 'Image',
        author: 'author',
    },
    {
        img: petHelp,
        title: 'Image',
        author: 'author',
    },
    {
        img: petSaw,
        title: 'Image',
        author: 'author',
    },
];

export function TitlebarGridList() {
    const classes = useStyles();

    return (
        <div className={classes.root}>
            <GridList cellHeight={180} className={classes.gridList}>
                <GridListTile key="Subheader" cols={2} style={{ height: 'auto' }}>
                    <ListSubheader component="div">December</ListSubheader>
                </GridListTile>
                {tileData.map(tile => (
                    <GridListTile key={tile.img}>
                        <img src={tile.img} alt={tile.title} />
                        <GridListTileBar
                            title={tile.title}
                            subtitle={<span>by: {tile.author}</span>}
                            actionIcon={
                                <IconButton aria-label={`info about ${tile.title}`} className={classes.icon}>
                                    <InfoIcon />
                                </IconButton>
                            }
                        />
                    </GridListTile>
                ))}
            </GridList>
        </div>
    );
}

interface PetSearchProps {

};

interface PetSearchState {

};

export class PetSearchPage extends React.Component<PetSearchProps, PetSearchState> {
    constructor(props: PetSearchProps) {
        super(props);
        this.state = {
        };
    }

    render() {
        return (
            <Button variant="contained" color="primary">
                Ищу питомца
            </Button>
        );
    }
}
