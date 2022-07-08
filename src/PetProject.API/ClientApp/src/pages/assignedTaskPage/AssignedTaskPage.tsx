import React from 'react';
import PropTypes from 'prop-types';
import clsx from 'clsx';
import { lighten, makeStyles } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TablePagination from '@material-ui/core/TablePagination';
import TableRow from '@material-ui/core/TableRow';
import TableSortLabel from '@material-ui/core/TableSortLabel';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import Paper from '@material-ui/core/Paper';
import Checkbox from '@material-ui/core/Checkbox';
import IconButton from '@material-ui/core/IconButton';
import Tooltip from '@material-ui/core/Tooltip';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Switch from '@material-ui/core/Switch';
import DeleteIcon from '@material-ui/icons/Delete';
import FilterListIcon from '@material-ui/icons/FilterList';
import { Theme, createStyles, WithStyles, withStyles } from '@material-ui/core';
import { connect } from 'react-redux';

export interface TableData {
    id: number, status: string, task: string, pet: string, user: string
}

function createData(id: number, status: string, task: string, pet: string, user: string) {
    return { id, status, task, pet, user };
}

const rows = [
    createData(1, 'В прогрессе', 'Покормить', 'Барсик', 'Петр'),
    createData(2, 'В прогрессе', 'Кастрация', 'Бэтмен', 'Генадий'),
    createData(3, 'Выполненно', 'Усыновление', 'Мурзик', 'Виктор'),
    createData(4, 'Отмененно', 'Покормить', 'Рекс', 'Евгений'),
    createData(5, 'Выполненно', 'Кастрация', 'Пушок', 'Юрий'),
];

function desc(a: any, b: any, orderBy: string) {
    if (b[orderBy] < a[orderBy]) {
        return -1;
    }
    if (b[orderBy] > a[orderBy]) {
        return 1;
    }
    return 0;
}

function stableSort(array: Array<TableData>, cmp: (a: any, b: any) => number) {
    const stabilizedThis = array.map((el: TableData, ind: number) => { return { element: el, index: ind } });
    stabilizedThis.sort((a, b) => {
        const order = cmp(a.element, b.element);
        if (order !== 0) return order;
        return a.index - b.index;
    });
    return stabilizedThis.map(el => el.element);
}

function getSorting(order: string, orderBy: string) {
    return order === 'desc' ? (a: any, b: any) => desc(a, b, orderBy) : (a: any, b: any) => -desc(a, b, orderBy);
}

const headCells = [
    { id: 'status', numeric: false, disablePadding: true, label: 'Статус задания' },
    { id: 'task', numeric: true, disablePadding: false, label: 'Задание' },
    { id: 'pet', numeric: true, disablePadding: false, label: 'Животное' },
    { id: 'user', numeric: true, disablePadding: false, label: 'Исполнитель' },
];

function EnhancedTableHead(props: any) {
    const { classes, onSelectAllClick, order, orderBy, numSelected, rowCount, onRequestSort } = props;
    const createSortHandler = (property: any) => (event: any) => {
        onRequestSort(event, property);
    };

    return (
        <TableHead>
            <TableRow>
                <TableCell padding="checkbox">
                    <Checkbox
                        indeterminate={numSelected > 0 && numSelected < rowCount}
                        checked={rowCount > 0 && numSelected === rowCount}
                        onChange={onSelectAllClick}
                        inputProps={{ 'aria-label': 'select all desserts' }}
                    />
                </TableCell>
                {headCells.map(headCell => (
                    <TableCell
                        key={headCell.id}
                        align={headCell.numeric ? 'right' : 'left'}
                        padding={headCell.disablePadding ? 'none' : 'default'}
                        sortDirection={orderBy === headCell.id ? order : false}
                    >
                        <TableSortLabel
                            active={orderBy === headCell.id}
                            direction={orderBy === headCell.id ? order : 'asc'}
                            onClick={createSortHandler(headCell.id)}
                        >
                            {headCell.label}
                            {orderBy === headCell.id ? (
                                <span className={classes.visuallyHidden}>
                                    {order === 'desc' ? 'sorted descending' : 'sorted ascending'}
                                </span>
                            ) : null}
                        </TableSortLabel>
                    </TableCell>
                ))}
            </TableRow>
        </TableHead>
    );
}

EnhancedTableHead.propTypes = {
    classes: PropTypes.object.isRequired,
    numSelected: PropTypes.number.isRequired,
    onRequestSort: PropTypes.func.isRequired,
    onSelectAllClick: PropTypes.func.isRequired,
    order: PropTypes.oneOf(['asc', 'desc']).isRequired,
    orderBy: PropTypes.string.isRequired,
    rowCount: PropTypes.number.isRequired,
};

const useToolbarStyles = makeStyles(theme => ({
    root: {
        paddingLeft: theme.spacing(2),
        paddingRight: theme.spacing(1),
    },
    highlight:
        theme.palette.type === 'light'
            ? {
                color: theme.palette.secondary.main,
                backgroundColor: lighten(theme.palette.secondary.light, 0.85),
            }
            : {
                color: theme.palette.text.primary,
                backgroundColor: theme.palette.secondary.dark,
            },
    title: {
        flex: '1 1 100%',
    },
}));

const EnhancedTableToolbar = (props: any) => {
    const classes = useToolbarStyles();
    const { numSelected } = props;

    return (
        <Toolbar
            className={clsx(classes.root, {
                [classes.highlight]: numSelected > 0,
            })}
        >
            {numSelected > 0 ? (
                <Typography className={classes.title} color="inherit" variant="subtitle1">
                    {numSelected} selected
        </Typography>
            ) : (
                    <Typography className={classes.title} variant="h6" id="tableTitle">
                        Список задач
        </Typography>
                )}

            {numSelected > 0 ? (
                <Tooltip title="Delete">
                    <IconButton aria-label="delete">
                        <DeleteIcon />
                    </IconButton>
                </Tooltip>
            ) : (
                    <Tooltip title="Filter list">
                        <IconButton aria-label="filter list">
                            <FilterListIcon />
                        </IconButton>
                    </Tooltip>
                )}
        </Toolbar>
    );
};

EnhancedTableToolbar.propTypes = {
    numSelected: PropTypes.number.isRequired,
};

const styles = (theme: Theme) => createStyles({
    root: {
        width: '100%',
    },
    paper: {
        width: '100%',
        marginBottom: theme.spacing(2),
    },
    table: {
        minWidth: 750,
    },
    visuallyHidden: {
        border: 0,
        clip: 'rect(0 0 0 0)',
        height: 1,
        margin: -1,
        overflow: 'hidden',
        padding: 0,
        position: 'absolute',
        top: 20,
        width: 1,
    },
});

interface AssignedTaskState {
    order: string,
    setOrder: string,
    orderBy: string,
    setOrderBy: string,
    selected: number[],
    setSelected: number[],
    page: number,
    setPage: number,
    rowsPerPage: number,
    setRowsPerPage: number
}

interface AssignedTaskProps extends WithStyles<typeof styles> {
}


const AssignedTaskPageStyled = withStyles(styles)(
    class AssignedTaskPage extends React.Component<AssignedTaskProps, AssignedTaskState> {
        constructor(props: AssignedTaskProps) {
            super(props);
            this.state = {
                order: 'asc',
                setOrder: 'asc',
                orderBy: 'status',
                setOrderBy: 'status',
                selected: [] as number[],
                setSelected: [] as number[],
                page: 0,
                setPage: 0,
                rowsPerPage: 5,
                setRowsPerPage: 5
            };
        }

        render() {
            const handleRequestSort = (event: any, property: any) => {
                const isAsc = this.state.orderBy === property && this.state.order === 'asc';
                this.setState({ setOrder: isAsc ? 'desc' : 'asc' });
                this.setState({ setOrderBy: property });
            };

            const handleSelectAllClick = (event: any) => {
                if (event.target.checked) {
                    const newSelecteds = rows.map(n => n.id);
                    this.setState({ setSelected: newSelecteds });
                    return;
                }
                this.setState({ setSelected: [] as number[] });
            };

            const handleClick = (event: any, id: number) => {
                const selectedIndex = this.state.selected.indexOf(id);
                let newSelected = [] as number[];

                if (selectedIndex === -1) {
                    newSelected = newSelected.concat(this.state.selected, id);
                } else if (selectedIndex === 0) {
                    newSelected = newSelected.concat(this.state.selected.slice(1));
                } else if (selectedIndex === this.state.selected.length - 1) {
                    newSelected = newSelected.concat(this.state.selected.slice(0, -1));
                } else if (selectedIndex > 0) {
                    newSelected = newSelected.concat(
                        this.state.selected.slice(0, selectedIndex),
                        this.state.selected.slice(selectedIndex + 1),
                    );
                }

                this.setState({ setSelected: newSelected });
            };

            const handleChangePage = (event: any, newPage: number) => {
                this.setState({ setPage: newPage });
            };

            const handleChangeRowsPerPage = (event: any) => {
                this.setState({ setRowsPerPage: parseInt(event.target.value, 10) });
                this.setState({ setPage: 0 });
            };

            const isSelected = (id: number) => this.state.selected.indexOf(id) !== -1;

            const emptyRows = this.state.rowsPerPage - Math.min(this.state.rowsPerPage, rows.length - this.state.page * this.state.rowsPerPage);

            const { classes } = this.props;
            return (
                <div className={classes.root}>
                    <Paper className={classes.paper}>
                        <EnhancedTableToolbar numSelected={this.state.selected.length} />
                        <TableContainer>
                            <Table
                                className={classes.table}
                                aria-labelledby="tableTitle"
                                size={'medium'}
                                aria-label="enhanced table"
                            >
                                <EnhancedTableHead
                                    classes={classes}
                                    numSelected={this.state.selected.length}
                                    order={this.state.order}
                                    orderBy={this.state.orderBy}
                                    onSelectAllClick={handleSelectAllClick}
                                    onRequestSort={handleRequestSort}
                                    rowCount={rows.length}
                                />
                                <TableBody>
                                    {stableSort(rows, getSorting(this.state.order, this.state.orderBy))
                                        .slice(this.state.page * this.state.rowsPerPage, this.state.page * this.state.rowsPerPage + this.state.rowsPerPage)
                                        .map((row, index) => {
                                            const isItemSelected = isSelected(row.id);
                                            const labelId = `enhanced-table-checkbox-${index}`;

                                            return (
                                                <TableRow
                                                    hover
                                                    onClick={event => handleClick(event, row.id)}
                                                    role="checkbox"
                                                    aria-checked={isItemSelected}
                                                    tabIndex={-1}
                                                    key={row.id}
                                                    selected={isItemSelected}
                                                >
                                                    <TableCell padding="checkbox">
                                                        <Checkbox
                                                            checked={isItemSelected}
                                                            inputProps={{ 'aria-labelledby': labelId }}
                                                        />
                                                    </TableCell>
                                                    <TableCell component="th" id={labelId} scope="row" padding="none">
                                                        {row.status}
                                                    </TableCell>
                                                    <TableCell align="right">{row.task}</TableCell>
                                                    <TableCell align="right"><a href="#">{row.pet}</a></TableCell>
                                                    <TableCell align="right"><a href="#">{row.user}</a></TableCell>
                                                </TableRow>
                                            );
                                        })}
                                    {emptyRows > 0 && (
                                        <TableRow style={{ height: 53 * emptyRows }}>
                                            <TableCell colSpan={6} />
                                        </TableRow>
                                    )}
                                </TableBody>
                            </Table>
                        </TableContainer>
                        <TablePagination
                            rowsPerPageOptions={[5, 10, 25]}
                            component="div"
                            count={rows.length}
                            rowsPerPage={this.state.rowsPerPage}
                            page={this.state.page}
                            onPageChange={handleChangePage}
                            onRowsPerPageChange={handleChangeRowsPerPage}
                        />
                    </Paper>
                </div>
            );
        }
    })

export const AssignedTaskPageConnected = connect()(AssignedTaskPageStyled);
